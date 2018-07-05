using System.Windows.Forms;

namespace XCI_Organizer.Helpers {
    public class BetterTreeNode : TreeNode {
        public long Offset;
        public long Size;
        public string ExpectedHash;
        public string ActualHash;
        public long HashedRegionSize;

        public BetterTreeNode(string t) {
            base.Text = t;
        }
    }
}
