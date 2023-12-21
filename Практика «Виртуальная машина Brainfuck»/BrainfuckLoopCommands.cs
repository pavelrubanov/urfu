using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        private static Dictionary<int, int> CloseOpencycle = new Dictionary<int, int>();
        private static Dictionary<int, int> OpenClosecycle = new Dictionary<int, int>();
        private static Stack<int> stack = new Stack<int>();
        public static void RegisterTo(IVirtualMachine vm)
        {
            for (int i = 0; i < vm.Instructions.Length; i++)
            {
                if (vm.Instructions[i] == '[') stack.Push(i);
                if (vm.Instructions[i] == ']')
                {
                    var index = stack.Pop();
                    CloseOpencycle[i] = index;
                    OpenClosecycle[index] = i;
                }
            }

            vm.RegisterCommand('[', b => 
            {
                if (b.Memory[b.MemoryPointer] == 0)
                    b.InstructionPointer = OpenClosecycle[b.InstructionPointer];
            });
            vm.RegisterCommand(']', b => 
            {
                if (b.Memory[b.MemoryPointer] != 0)
                    b.InstructionPointer = CloseOpencycle[b.InstructionPointer];
            });
        }
    }

}
