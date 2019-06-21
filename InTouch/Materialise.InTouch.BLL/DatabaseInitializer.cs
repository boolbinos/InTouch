using Materialise.InTouch.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Materialise.InTouch.BLL
{
    public class DatabaseInitializer
    {
        private readonly InTouchContext _dbContext;

        public DatabaseInitializer(InTouchContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.Migrate();
        }
    }
}