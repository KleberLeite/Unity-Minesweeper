
namespace Minesweeper.Databases
{
    public interface IData
    {
        int ID { get; set; }
        object GetValue();
    }
}
