using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Xml;
using ExcelDataReader;
using UnityEditor;

public class ExcelToJsonConverter 
{	
	public delegate void ConversionToJsonSuccessfullHandler();
	public event ConversionToJsonSuccessfullHandler ConversionToJsonSuccessfull = delegate {};
	
	public delegate void ConversionToJsonFailedHandler();
	public event ConversionToJsonFailedHandler ConversionToJsonFailed = delegate {};

    /*public void ConvertExcelFileToJson(string inputPath, string outputPath, Dictionary<string, Dictionary<string, string>> localizationOverrides)
    {
        var absInputPath = Application.dataPath + "/" + inputPath;
        var absOutputPath = Application.dataPath + "/" + outputPath;
        
        Debug.Log("Excel To Json Converter: Processing: " + absInputPath);
        
        var excelData = FormatExcelData(absInputPath);
        var spreadSheetTable = GetSpreadSheetJson(excelData, excelData.Tables[0].TableName);

        // do overwriting
        var processed = new List<string>();
        foreach (DataRow row in spreadSheetTable.Rows)
        {
            var key = row[0].ToString();
            if (localizationOverrides.ContainsKey(key))
            {
                var rowOverride = localizationOverrides.First(lo => lo.Key == key);

                foreach (var rowColumnOverride in rowOverride.Value)
                {
                    row[rowColumnOverride.Key] = rowColumnOverride.Value;
                }

                processed.Add(key);
            }
        }

        if (processed.Count != localizationOverrides.Count)
        {
            var unprocessed = localizationOverrides.Where(lo => !processed.Contains(lo.Key)).Select(lo => lo.Key).ToArray();
            Debug.LogError("Couldn't find keys for: " + string.Join(", ", unprocessed));
        }
        
        var spreadSheetJsonString = JsonConvert.SerializeObject(spreadSheetTable);
        var formattedSpreadSheetJsonString = FormatJson(spreadSheetJsonString);
        
        WriteTextToFile(formattedSpreadSheetJsonString, absOutputPath);
        Debug.Log("Excel To Json Converter: " + excelData.Tables[0].TableName + " successfully written to " + absOutputPath);

        AssetDatabase.Refresh();
    }*/

    /// <summary>
    /// Converts all excel files in the input folder to json and saves them in the output folder.
    /// Each sheet within an excel file is saved to a separate json file with the same name as the sheet name.
    /// Files, sheets and columns whose name begin with '~' are ignored.
    /// </summary>
    /// <param name="inputPath">Input path.</param>
    /// <param name="outputPath">Output path.</param>
    /// <param name="recentlyModifiedOnly">If set to <c>true</c>, will only process recently modified files only.</param>
    public void ConvertExcelFilesToJson(string inputPath, string outputPath, bool recentlyModifiedOnly = false)
	{
		List<string> excelFiles = GetExcelFileNamesInDirectory(inputPath);
		Debug.Log("Excel To Json Converter: " + excelFiles.Count.ToString() + " excel files found.");
		
		if (recentlyModifiedOnly)
		{
			excelFiles = RemoveUnmodifiedFilesFromProcessList(excelFiles, outputPath);
			
			if (excelFiles.Count == 0)
			{
				Debug.Log("Excel To Json Converter: No updates to excel files since last conversion.");
			}
			else
			{
				Debug.Log("Excel To Json Converter: " + excelFiles.Count.ToString() + " excel files updated/added since last conversion.");
			}
		}
		
		bool succeeded = true;
		
		for (int i = 0 ; i < excelFiles.Count; i++)
		{
			if (!ConvertExcelFileToJson(excelFiles[i], outputPath))
			{
				succeeded = false;
				break;
			}
		}
		
		if (succeeded)
		{
			ConversionToJsonSuccessfull();
		}
		else
		{
			ConversionToJsonFailed();
		}
	}
	
	/// <summary>
	/// Gets all the file names in the specified directory
	/// </summary>
	/// <returns>The excel file names in directory.</returns>
	/// <param name="directory">Directory.</param>
	private List<string> GetExcelFileNamesInDirectory(string directory)
	{
		string[] directoryFiles = Directory.GetFiles(directory);
		List<string> excelFiles = new List<string>();
		
		// Regular expression to match against 2 excel file types (xls & xlsx), ignoring
		// files with extension .meta and starting with ~$ (temp file created by excel when fie
		Regex excelRegex = new Regex(@"^((?!(~\$)).*\.(xlsx|xls$))$");
		
		for (int i = 0; i < directoryFiles.Length; i++)
		{
			string fileName = directoryFiles[i].Substring(directoryFiles[i].LastIndexOf('/') + 1);
			
			if (excelRegex.IsMatch(fileName))
			{
				excelFiles.Add(directoryFiles[i]);
			}
		}
		
		return excelFiles;
	}
	
	/// <summary>
	/// Converts each sheet in the specified excel file to json and saves them in the output folder.
	/// The name of the processed json file will match the name of the excel sheet. Ignores
	/// sheets whose name begin with '~'. Also ignores columns whose names begin with '~'.
	/// </summary>
	/// <returns><c>true</c>, if excel file was successfully converted to json, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	/// <param name="outputPath">Output path.</param>
	public bool ConvertExcelFileToJson(string filePath, string outputPath)
	{
		Debug.Log("Excel To Json Converter: Processing: " + filePath);
		var formattedTables = FormatExcelData(filePath);
		
		if (!formattedTables.Any())
		{
			Debug.LogError("Excel To Json Converter: Failed to process file: " + filePath);
			return false;
		}
		
		// Todo change writer structure to keep dictionary and modify the reader to handle dictionaries
	    foreach (var formattedTable in formattedTables)
	    {
	        var previousFormat = formattedTable.Value.Select(locByLangByKey =>
	        {
	            var concat = new Dictionary<string, string>
	            {
	                {"Key", locByLangByKey.Key}
	            };

	            foreach (var locByLang in locByLangByKey.Value)
	            {
	                concat.Add(locByLang.Key, locByLang.Value);
	            }

	            return concat;
	        }).ToList();

            var spreadSheetJsonString = JsonConvert.SerializeObject(previousFormat);

            if (string.IsNullOrEmpty(spreadSheetJsonString))
			{
				Debug.LogError("Excel To Json Converter: Failed to covert Spreadsheet '" + formattedTable.Key + "' to json.");
				return false;
			}
			else
			{
				// The file name is the sheet name with spaces removed
				var fileName = formattedTable.Key.Replace(" ", string.Empty);
			    spreadSheetJsonString = FormatJson(spreadSheetJsonString);
				WriteTextToFile(spreadSheetJsonString, outputPath + "/" + fileName + ".json");
				Debug.Log("Excel To Json Converter: " + formattedTable.Key + " successfully written to file.");
			}
		}
		
		return true;
	}

    public string FormatJson(string unformatted)
    {
        return unformatted.Replace("},{", "}," + Environment.NewLine + "{");
    }
	
	/// <summary>
	/// Gets the excel data reader for the specified file.
	/// </summary>
	/// <returns>The excel data reader for file or null if file type is invalid.</returns>
	/// <param name="filePath">File path.</param>
	private IExcelDataReader GetExcelDataReaderForFile(string filePath)
	{
		var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		
		// Create the excel data reader
		IExcelDataReader excelReader;
		
		// Create regular expressions to detect the type of excel file
		Regex xlsRegex = new Regex(@"^(.*\.(xls$))");
		Regex xlsxRegex = new Regex(@"^(.*\.(xlsx$))");
		
		// Read the excel file depending on it's type
		if (xlsRegex.IsMatch(filePath))
		{
			// Reading from a binary Excel file ('97-2003 format; *.xls)
			excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		}
		else if (xlsxRegex.IsMatch(filePath))
		{
			// Reading from a OpenXml Excel file (2007 format; *.xlsx)
			excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		}
		else
		{
			Debug.LogError("Excel To Json Converter: Unexpected files type: " + filePath);
			stream.Close();
			return null;
		}
		
		// Close the stream
		//stream.Close();
		
		// First row are columns names
		//excelReader.IsFirstRowAsColumnNames = true;
		
		return excelReader;
	}
	
	/// <summary>
	/// Gets the Excel data from the specified file
	/// </summary>
	/// <returns>The excel data set or null if file is invalid.</returns>
	/// <param name="filePath">File path.</param>
	private List<KeyValuePair<string, Dictionary<string, Dictionary<string, string>>>> FormatExcelData(string filePath)
	{
	    var formattedTables = new List<KeyValuePair<string, Dictionary<string, Dictionary<string, string>>>>();

        // Get the excel data reader with the excel data
        using (var excelReader = GetExcelDataReaderForFile(filePath))
	    {
	        if (excelReader == null)
	        {
	            return null;
	        }

	        do
	        {
	            // Get the DataTable from the current spreadsheet
	            var formattedData = GetExcelSheetData(excelReader);

	            if (formattedData != null)
	            {
	                formattedTables.Add(
	                    new KeyValuePair<string, Dictionary<string, Dictionary<string, string>>>(
	                        excelReader.Name,
	                        formattedData));
	            }
	        } while (excelReader.NextResult()); // Read the next sheet

	        return formattedTables;
        }
	}
	
	/// <summary>
	/// Gets the Excel data from current spreadsheet
	/// </summary>
	/// <returns>The spreadsheet data table.</returns>
	/// <param name="excelReader">Excel Reader.</param>
	private Dictionary<string, Dictionary<string, string>> GetExcelSheetData(IExcelDataReader excelReader)
	{
		if (excelReader == null)
		{
			Debug.LogError("Excel To Json Converter: Excel Reader is null. Cannot read data");
			return null;
		}
		
		// Ignore sheets which start with ~
		Regex sheetNameRegex = new Regex(@"^~.*$");
		if (sheetNameRegex.IsMatch(excelReader.Name))
		{
			return null;
		}
		
        var languageColumnMappings = new Dictionary<int, string>();
        var formattedData = new Dictionary<string, Dictionary<string, string>>();


        // todo: Implement a version of the funcitonality below
        /*// Remove columns which start with '~'
	        Regex columnNameRegex = new Regex(@"^~.*$");
	    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
	    {
	        if (columnNameRegex.IsMatch(dataTable.Columns[i].ColumnName))
	        {
	            dataTable.Columns.RemoveAt(i);
	        }
	    }*/

        // Read the rows and columns
        while (excelReader.Read())
	    {
	        if (excelReader.Depth == 0)
	        {
	            for (int i = 1; i < excelReader.FieldCount; i++)
	            {
	                var language = excelReader.IsDBNull(i) ? "" : excelReader.GetString(i);

	                if (!string.IsNullOrWhiteSpace(language))
	                {
	                    languageColumnMappings.Add(i, language);
	                }
	            }
	        }
	        else
	        {
	            var key = excelReader.IsDBNull(0) ? "" : excelReader.GetString(0);
	            if (!string.IsNullOrWhiteSpace(key))
	            {
	                var keyLocalizations = new Dictionary<string, string>();
	                try
	                {
	                    formattedData.Add(key, keyLocalizations);

	                    for (int i = 1; i < excelReader.FieldCount; i++)
	                    {
	                        var localization = excelReader.IsDBNull(i) ? "" : excelReader.GetString(i);
	                        if (!string.IsNullOrWhiteSpace(localization))
	                        {
	                            var language = languageColumnMappings[i];
	                            keyLocalizations.Add(language, localization);
	                        }
	                    }
	                }
	                catch (ArgumentException)
	                {
                        Debug.LogError($"Key: {key}");
                        throw;
	                }
	            }
	        }
	    }

        return formattedData;
	}
	
	/// <summary>
	/// Gets the json data for the specified spreadsheet in the specified DataSet
	/// </summary>
	/// <returns>The spread sheet json.</returns>
	/// <param name="excelDataSet">Excel data set.</param>
	/// <param name="sheetName">Sheet name.</param>
	private DataTable GetSpreadSheetJson(DataSet excelDataSet, string sheetName)
	{
		// Get the specified table
		DataTable dataTable = excelDataSet.Tables[sheetName];

	    // Remove empty rows
	    for (var row = dataTable.Rows.Count - 1; row >= 0; row--)
	    {
	        if (dataTable.Rows[row].ItemArray.All(r => string.IsNullOrWhiteSpace(r.ToString())))
	        {
	            dataTable.Rows.RemoveAt(row);
	        }
	    }

        // Remove empty columns
        for (int col = dataTable.Columns.Count - 1; col >= 0; col--)
		{
			bool removeColumn = true;
			foreach (DataRow row in dataTable.Rows)
			{
				if (!row.IsNull(col))
				{
					removeColumn = false;
					break;
				}
			}
			
			if (removeColumn)
			{
				dataTable.Columns.RemoveAt(col);
			}
		}

	    // Remove columns which start with '~'
        Regex columnNameRegex = new Regex(@"^~.*$");
		for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
		{
			if (columnNameRegex.IsMatch(dataTable.Columns[i].ColumnName))
			{
				dataTable.Columns.RemoveAt(i);
			}
		}
		
		return dataTable;
	}
	
	/// <summary>
	/// Writes the specified text to the specified file, overwriting it.
	/// Creates file if it does not exist.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="filePath">File path.</param>
	private void WriteTextToFile(string text, string filePath)
	{
		File.WriteAllText(filePath, text);
	}
	
	/// <summary>
	/// Removes files which have not been modified since they were last processed
	/// from the process list
	/// </summary>
	/// <param name="excelFiles">Excel files.</param>
	private List<string> RemoveUnmodifiedFilesFromProcessList(List<string> excelFiles, string outputDirectory)
	{
		List<string> sheetNames;
		bool removeFile = true;
		
		// ignore sheets whose name starts with '~'
		Regex sheetNameRegex = new Regex(@"^~.*$");
		
		for (int i = excelFiles.Count - 1; i >= 0; i--)
		{
			sheetNames = GetSheetNamesInFile(excelFiles[i]);
			removeFile = true;
			
			for (int j = 0; j < sheetNames.Count; j++)
			{
				if (sheetNameRegex.IsMatch(sheetNames[j]))
				{
					continue;
				}
				
				string outputFile = outputDirectory + "/" + sheetNames[j] + ".json";
				if (!File.Exists(outputFile) ||
				    File.GetLastWriteTimeUtc(excelFiles[i]) > File.GetLastWriteTimeUtc(outputFile))
				{
					removeFile = false;
				}
			}
			
			if (removeFile)
			{
				excelFiles.RemoveAt(i);
			}
		}
		
		return excelFiles;
	}
	
	/// <summary>
	/// Gets the list of sheet names in the specified excel file
	/// </summary>
	/// <returns>The sheet names in file.</returns>
	/// <param name="filePath">File path.</param>
	private List<string> GetSheetNamesInFile(string filePath)
	{
		List<string> sheetNames = new List<string>();
		
		// Get the excel data reader with the excel data
		IExcelDataReader excelReader = GetExcelDataReaderForFile(filePath);
		
		if (excelReader == null)
		{
			return sheetNames;
		}
		
		do
		{
			// Add the sheet name to the list
			sheetNames.Add(excelReader.Name);
		}
		while(excelReader.NextResult()); // Read the next sheet
		
		return sheetNames;
	}
}
