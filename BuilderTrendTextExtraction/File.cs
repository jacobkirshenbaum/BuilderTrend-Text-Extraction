/**
 * Object for files that will be inserted into Elasticsearch.
 * Encapsulates the relevant data for a single file (including
 * its identifier, filename, text, and information for filters).
 */
public class File
{
    /**
     * Unique identification number for this file.
     */
    public int? ID { get; set; }
    
    /**
     * The name of the file.
     */
    public string? FileName { get; set; }

    /**
     * The text that was extracted from the file, stored as a single string.
     */
    public string? Text { get; set;  }
    
    /**
     * A list of the addresses found in the file.
     */
    public List<string>? Addresses { get; set; }
    
    /**
     * A list of phone numbers found in the file.
     */
    public List<string>? PhoneNumbers { get; set; }
    
    /**
     * A list of emails found in the file.
     */
    public List<string>? Emails { get; set; }
}
