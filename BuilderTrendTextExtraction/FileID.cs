/**
 * A small utility class that generates identifiers for file objects.
 */
public class FileID
{
    /**
     * The next identifier to be generated.
     */
    private static int id = 1;

    /**
     * Increments ID ensuring a unique ID for each file that has been processed.
     * @return Returns a unique ID.
     */
    public static int newId()
    {
        return id++;
    }
}