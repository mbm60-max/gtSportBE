using PDTools.SimulatorInterface;
namespace FullSimulatorPacket{
   internal class ExtendedPacket : SimulatorPacket
{
    public float distanceFromStart { get; set;}
    public bool IsSpecial { get; set; }
    public void AdditionalMethod()
    {
        // Additional behavior
    }
} 
}