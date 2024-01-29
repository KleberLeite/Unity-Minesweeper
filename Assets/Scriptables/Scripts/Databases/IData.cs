
namespace Minesweeper.Databases
{
    public interface IData
    {
        int ID { get; }
        object GetValue();
    }
}
