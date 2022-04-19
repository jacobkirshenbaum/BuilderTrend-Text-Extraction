using System;
using System.Globalization;
using System.Text.RegularExpressions;

public static class RegexUtilities
{
    // Should work for the two different zipcode styles: 12345 (5 digits) or 12345-1234 (5 digits - 4 digits)
    public static readonly string Zipcode = @"\b(\d{5}(?:-\d{4})?)\b";
    
    // Matches a US state full name
    public static readonly string FullState = @"Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming";
    
    // Matches a US state 2 letter abbreviation
    public static readonly string ShortState = @"AK|AL|AR|AZ|CA|CO|CT|DE|FL|GA|HI|IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VA|VT|WA|WI|WV|WY";
    
    // Matches a state, either full name or 2 letter abbbreviation
    public static readonly string State = @"\b(?:" + ShortState + @"|" + FullState + @")\b";
    
    // Matches a very generic address in the US. The basic structure of this address is the following:
    //      3-5 digit number, some non-greedy block of characters that may or may not include one 
    //      newline, a state, non-greedy block of characters, and a zipcode
    public static readonly string Address = @"(\d{3,5}.*?(\n)?.*?" + State + @".*?" + Zipcode + @")";
    
    // Should match an email
    public static readonly string Email = @"[^@\s]+@[^@\s]+\.[^@\s]+";
    
    // Should match a phone number
    public static readonly string PhoneNumber = @"(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}";
    
}