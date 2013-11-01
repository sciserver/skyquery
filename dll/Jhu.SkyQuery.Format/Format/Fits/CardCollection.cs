using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    public class CardCollection : IDictionary<string, Card>, IList<Card>
    {
        private List<Card> cardsList;
        private Dictionary<string, Card> cardsDictionary;

        public CardCollection()
        {
            InitializeMembers();
        }

        public CardCollection(CardCollection old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.cardsList = new List<Card>();
            this.cardsDictionary = new Dictionary<string, Card>(StringComparer.InvariantCultureIgnoreCase);
        }

        private void CopyMembers(CardCollection old)
        {
            this.cardsList = new List<Card>(old.cardsList);
            this.cardsDictionary = new Dictionary<string, Card>(old.cardsDictionary);
        }

        private void ValidateKeyword(string key, Card value)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Compare(key, value.Keyword) == 0)
            {
                return;
            }
            else
            {
                throw new ArgumentException("Keyword and key don't match.");
            }
        }

        public void Add(Card value)
        {
            if (cardsDictionary.ContainsKey(value.Keyword))
            {
                throw new ArgumentException("Duplicate key");
            }

            cardsList.Add(value);
            cardsDictionary.Add(value.Keyword, value);
        }

        public void Add(string key, Card value)
        {
            ValidateKeyword(key, value);
            Add(value);
        }

        public bool ContainsKey(string key)
        {
            return cardsDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return cardsDictionary.Keys; }
        }

        public bool Remove(string key)
        {
            cardsList.Remove(cardsDictionary[key]);
            return cardsDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out Card value)
        {
            return cardsDictionary.TryGetValue(key, out value);
        }

        public ICollection<Card> Values
        {
            get { return cardsList; }
        }

        public Card this[string key]
        {
            get
            {
                return cardsDictionary[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, Card> item)
        {
            ValidateKeyword(item.Key, item.Value);

            Add(item.Value);
        }

        public void Clear()
        {
            cardsList.Clear();
            cardsDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, Card> item)
        {
            return cardsDictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, Card>>.CopyTo(KeyValuePair<string, Card>[] array, int arrayIndex)
        {
            ((IDictionary<string, Card>)cardsDictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return cardsList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, Card> item)
        {
            return Remove(item.Key);
        }

        IEnumerator<KeyValuePair<string, Card>> IEnumerable<KeyValuePair<string, Card>>.GetEnumerator()
        {
            return ((IDictionary<string, Card>)cardsDictionary).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, Card>)cardsDictionary).GetEnumerator();
        }

        public int IndexOf(Card item)
        {
            return cardsList.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            cardsDictionary.Add(item.Keyword, item);
            cardsList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            cardsDictionary.Remove(cardsList[index].Keyword);
            cardsList.RemoveAt(index);
        }

        public Card this[int index]
        {
            get
            {
                return cardsList[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Contains(Card item)
        {
            return cardsList.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Card item)
        {
            cardsDictionary.Remove(item.Keyword);
            return cardsList.Remove(item);
        }

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return cardsList.GetEnumerator();
        }
    }
}
