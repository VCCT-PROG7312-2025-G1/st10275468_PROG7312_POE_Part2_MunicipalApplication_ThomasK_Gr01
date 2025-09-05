namespace Municipal_Services_Portal.Models
{

    public class IssueNode
    {
        public Issue Data { get; set; }
        public IssueNode Next { get; set; }

        public IssueNode(Issue issue)
        {
            Data = issue;
            Next = null;
        }
    }

    public class IssueLinkedList
    {
        private IssueNode head;

        public IssueLinkedList()
        {
            head = null;
        }

        public void AddIssue(Issue issue)
        {
            IssueNode newNode = new IssueNode(issue);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                IssueNode current = head;

                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public Issue[] ToArray()
        {
            int count = 0;
            IssueNode current = head;
            while (current != null)
            {
                count++;
                current = current.Next;
            }

            Issue[] issuesArray = new Issue[count];

            current = head;
            int index = 0;
            while (current != null)
            {
                issuesArray[index++] = current.Data;
                current = current.Next;
            }

            return issuesArray;
        }

    }
}
