/**
 * This is a collection of regular expression pattern strings that are all related to filtering desired
 * information out of text. They are necessary to achieve the filtering functionality in the GUI.
 */
public static class RegexUtilities
{
    /**
     * Should work for the two different zipcode styles:
     * 12345 (5 digits) or 12345-1234 (5 digits, hyphen, 4 digits)
     */
    public static readonly string Zipcode = @"\b(\d{5}(?:-\d{4})?)\b";
    
    /**
     * Matches a US state full name.
     * Structurally it is a list of all the state names.
     */
    public static readonly string FullState = @"Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming";
    
    /**
     * Matches a US state 2 letter abbreviation.
     * Structurally it is a list of all the state 2 letter abbreviations.
     */
    public static readonly string ShortState = @"AK|AL|AR|AZ|CA|CO|CT|DE|FL|GA|HI|IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VA|VT|WA|WI|WV|WY";
    
    /**
     * Matches a state, either full name or 2 letter abbbreviation.
     * Structurally it is a word boundary, either a ShortState or a FullState, and a word boundary.
     * By ShortState and FullState, it uses exactly the regexes defined in this class called ShortState and FullState
     */
    public static readonly string State = @"\b(?:" + ShortState + @"|" + FullState + @")\b";
    
    /**
     * Matches a very generic address in the US. The basic structure of this address is the following:
     * 3-5 digit number, some non-greedy block of characters that may or may not include one
     * newline, a state, non-greedy block of characters that doesn't include newline, and a zipcode.
     * By State and Zipcode, it uses exactly the regexes defined in this class called State and Zipcode
     */
    public static readonly string Address = @"(\d{3,5}.*?(\n)?.*?" + State + @".*?" + Zipcode + @")";
    
    /**
     * Should match an email.
     * Structurally, it matches one or more sequential characters (excluding @ and whitespace), the
     * @ character, one or more sequential characters (excluding @ and whitespace), a dot character,
     * and one or more sequential characters (excluding $ and whitespace)
     */
    public static readonly string Email = @"[^@\s]+@[^@\s]+\.[^@\s]+";
    
    /**
     * Should match a phone number.
     * Structurally, it matches the following:
     * Conutry code: an optional section that consists of a + character, a number with one or two digits, and a whitespace character
     * Area code: An optional ( character, 3 digits, and an optional ) character
     * 7 digit number and separators: a separator, 3 digits, a separator, and 4 digits
     * (Each of these separators are one of the following: a whitespace character, the . character, or the - character)
     */
    public static readonly string PhoneNumber = @"(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}";
    
}