using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Entities.Shop;
using SQLite;

namespace Client.Data
{
    public class ShopDB
    {
        private readonly SQLiteAsyncConnection _db;

        public ShopDB(string connectionString)
        {
            _db = new SQLiteAsyncConnection(connectionString);

            _db.CreateTableAsync<Shop>().GetAwaiter().GetResult();
        }
        
        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            return await _db.Table<Shop>().ToListAsync();
        }
        
        public async Task<Shop> GetById(int id)
        {
            return await _db.Table<Shop>().FirstOrDefaultAsync(x => x.Id == id);
        } 

        public async Task<Shop> GetByName(string name)
        {
            return await _db.Table<Shop>().FirstOrDefaultAsync(x => x.Name == name);
        }
        
        public async Task InsertShopsFromServer(IEnumerable<Shop> shops)
        {
            foreach (var shop in shops)
            {
                await _db.InsertOrReplaceAsync(shop);
            }
        }

        public async Task DeleteAllShops()
        {
            await _db.DeleteAllAsync<Shop>();
        }
    }
}