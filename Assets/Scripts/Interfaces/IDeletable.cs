using Data;

namespace Interfaces
{
    public interface IDeletable
    {
        public void Delete(GlobalData data, int id)
        {
            data.Edit<WorldData>(worldData => worldData.DeletedObjects.Add(id));
        }
    }
}