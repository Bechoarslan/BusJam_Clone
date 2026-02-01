using Runtime.Enums;

namespace Runtime.Interfaces
{
    public interface IPassenger
    {
        PassengerColorType ColorType { get; }
        void ChangeOutLine(float intensity);
        
    }
}