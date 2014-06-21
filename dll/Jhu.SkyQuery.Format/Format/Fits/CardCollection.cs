using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    public class CardCollection : IDictionary<string, Card>, IList<Card>
    {
        #region Private member variables

        private List<Card> cardsList;
        private Dictionary<string, Card> cardsDictionary;

        #endregion
        #region Properties and indexers

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

        public ICollection<Card> Values
        {
            get { return cardsList; }
        }

        public ICollection<string> Keys
        {
            get { return cardsDictionary.Keys; }
        }

        public int Count
        {
            get { return cardsList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
        #region Constructors and initializers

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

        #endregion
        #region Collection interface functions

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
            cardsDictionary.Add(value.Keyword, value);
            cardsList.Add(value);
        }

        public void Add(string key, Card value)
        {
            ValidateKeyword(key, value);
            Add(value);
        }

        public void Add(KeyValuePair<string, Card> item)
        {
            ValidateKeyword(item.Key, item.Value);

            Add(item.Value);
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

        public bool Remove(string key)
        {
            cardsList.Remove(cardsDictionary[key]);
            return cardsDictionary.Remove(key);
        }

        public bool Remove(Card item)
        {
            cardsDictionary.Remove(item.Keyword);
            return cardsList.Remove(item);
        }

        public bool Remove(KeyValuePair<string, Card> item)
        {
            return Remove(item.Key);
        }

        public void Clear()
        {
            cardsList.Clear();
            cardsDictionary.Clear();
        }

        public bool ContainsKey(string key)
        {
            return cardsDictionary.ContainsKey(key);
        }

        public bool Contains(Card item)
        {
            return cardsList.Contains(item);
        }

        public bool Contains(KeyValuePair<string, Card> item)
        {
            return cardsDictionary.Contains(item);
        }

        public int IndexOf(Card item)
        {
            return cardsList.IndexOf(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, Card>>.CopyTo(KeyValuePair<string, Card>[] array, int arrayIndex)
        {
            ((IDictionary<string, Card>)cardsDictionary).CopyTo(array, arrayIndex);
        }

        #endregion
        #region Value get functions

        /// <summary>
        /// Returns the value a header, if it exists.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out Card value)
        {
            return cardsDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns the value of an indexed header, if it exists.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, int index, out Card card)
        {
            key += index.ToString();
            return cardsDictionary.TryGetValue(key, out card);
        }

        #endregion
        #region Enumerator functions

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return cardsList.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, Card>> IEnumerable<KeyValuePair<string, Card>>.GetEnumerator()
        {
            return ((IDictionary<string, Card>)cardsDictionary).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, Card>)cardsDictionary).GetEnumerator();
        }

        #endregion
    }
}
