using System.Collections;
public class TrackList : ArrayList
{
    public new void Add(Track track)
    {
        base.Add(track);
    }

    public new void Remove(Track track)
    {
        base.Remove(track);
    }

    // Add other methods or custom functionality as needed
}
