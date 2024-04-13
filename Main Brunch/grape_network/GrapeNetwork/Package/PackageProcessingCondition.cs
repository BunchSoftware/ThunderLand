namespace GrapeNetwork.Packages
{
    public class PackageProcessingCondition
    {
        private ushort GroupCommand;
        private uint Command;

        public PackageProcessingCondition(ushort GroupCommand, uint Command)
        {
            this.GroupCommand = GroupCommand;
            this.Command = Command;
        }

        public bool CheckCondition(Package package)
        {
            if (package.GroupCommand == GroupCommand
                && package.Command == Command)
                return true;

            return false;
        }
    }
}
