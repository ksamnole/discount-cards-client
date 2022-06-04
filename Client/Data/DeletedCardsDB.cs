using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities.Card;
using SQLite;

namespace Client.Data
{
    public class DeletedCardsDB
    {
        private readonly SQLiteAsyncConnection _db;

        public DeletedCardsDB(string connectionString)
        {
            _db = new SQLiteAsyncConnection(connectionString);

            _db.CreateTableAsync<DeletedCard>().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<DeletedCard>> GetAllAsync()
        {
            return await _db.Table<DeletedCard>().ToListAsync();
        }

        public async Task DeleteCardByNumber(string number)
        {
            await _db.Table<DeletedCard>().DeleteAsync(x => x.Number == number);
        }

        public async Task AddAsync(Card card)
        {
            await _db.InsertAsync(new DeletedCard()
            {
                Id = card.Id,
                Number = card.Number
            });
        }
    }
}