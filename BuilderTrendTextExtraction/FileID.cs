public class FileID
{
    private static int id = 1;

    /**
     * Increments ID ensuring a unique ID for each file that has been processed 
     */
    public static int newId()
    {
        return id++;
    }
}