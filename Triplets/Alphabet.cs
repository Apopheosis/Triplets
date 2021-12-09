    using System.Collections;

    class Letter
    {
        public Letter(char let)
        {
            this.character = let;
        }
        public char character;
        public int repetition;
    
        public Letter next;
        public Letter prev;
    }
    
    class Alphabet : IEnumerable
    {
        public Letter head;
        public Letter tail;
    
        public int count { get; set; }
    
        public IEnumerator GetEnumerator()
        {
            Letter current = head;
            while(current.next!=null){
                yield return current;
                current = current.next;
            }
        }
    
        public void Add(Letter letter)
        {
            if (head == null)
            {
                head = letter;
            }
            else
            {
                tail.next = letter;
                letter.prev = tail;
                count++;
            }
    
            tail = letter;
            count++;
        }
    
        public bool Remove(char let)
        {
            Letter current = head;
            while (current != null)
            {
                if (current.character.Equals(let))
                {
                    break;
                }
    
                current = current.next;
            }
    
            if (current != null)
            {
                current.next.prev = current.prev;
            }
            else
            {
                tail = current.prev;
            }
    
            if (current.prev != null)
            {
                current.prev.next = current.next;
            }
            else
            {
                head = current.next;
            }
    
            count--;
            return true;
        }
    
        public bool Contains(char letter)
        {
            Letter current = head;
            while (current != null)
            {
                if (current.character.Equals(letter))
                {
                    return true;
                }
    
                current = current.next;
            }
    
            return false;
        }
    }