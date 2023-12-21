using System;
using System.Text;

namespace hashes;

public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
{
    private byte[] array = { 1, 2, 3, 5, 4 };
    private Vector vector = new Vector(0, 1);
    private Segment segment = new Segment(new Vector(0, 0), new Vector(1, 1));
    private Cat cat = new Cat("Nikita", "Dog", new DateTime(2004, 05, 25));
    private Robot robot = new Robot("", 1337);

    public void DoMagic()
    {
        array[1] = 10;
        cat.Rename("Butyaev");
        Robot.BatteryCapacity += 10000;
        vector = vector.Add(new Vector(1488, 1337));
        segment.End.Add(new Vector(1488, 1337));
    }

    Vector IFactory<Vector>.Create()
    {
        return vector;
    }

    Segment IFactory<Segment>.Create()
    {
        return segment;
    }

    Document IFactory<Document>.Create()
    {
        return new Document("Butyaev", Encoding.Unicode, array); ;
    }

    Cat IFactory<Cat>.Create()
    {
        return cat;
    }

    Robot IFactory<Robot>.Create()
    {
        return robot;
    }
}
