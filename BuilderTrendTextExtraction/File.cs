// "Object" for files that will be inserted into Elasticsearch
public class File
{
    public int? ID { get; set; }
    public string? FileName { get; set; }

    public string? Text { get; set;  }
    
    public List<string>? Addresses { get; set; }
    
    public List<string>? PhoneNumbers { get; set; }
    
    public List<string>? Emails { get; set; }
}
