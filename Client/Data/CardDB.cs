using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities.Card;
using SQLite;

namespace Client.Data
{
    public class CardDB
    {
        private readonly SQLiteAsyncConnection _db;

        public CardDB(string connectionString)
        {
            _db = new SQLiteAsyncConnection(connectionString);

            _db.CreateTableAsync<Card>().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<Card>> GetOrderedByDistanceCardsAsync()
        {
            return await _db.Table<Card>().OrderBy(x => x.LastDistanceToShop).ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetNotSyncCards()
        {
            return await _db.Table<Card>().Where(it => it.IsSync == false).ToListAsync();
        }

        public async Task InsertCardsFromServer(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                if (await _db.Table<Card>().FirstOrDefaultAsync(x => x.Number == card.Number) == null)
                {
                    await _db.InsertAsync(card);
                }
            }
        }

        public async Task<bool> IsShopUnique(string shopName)
        {
            return await _db.Table<Card>().FirstOrDefaultAsync(x => x.ShopName == shopName) == null;
        }
        
        public async Task<bool> IsCardNumberUnique(string number)
        {
            return await _db.Table<Card>().FirstOrDefaultAsync(x => x.Number == number) == null;
        }

        public async Task AddNewCard(Card card)
        {
            await _db.InsertAsync(card);
        }

        public async Task UpdateCard(Card card)
        {
            await _db.UpdateAsync(card);
        }
        
        public async Task DeleteCardByNumber(string number)
        {
            await _db.Table<Card>().DeleteAsync(x => x.Number == number);
        }
        
        public async Task DeleteAllCards()
        {
            await _db.DeleteAllAsync<Card>();
        }
    }
}