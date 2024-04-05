using System.Runtime.InteropServices;

namespace libdebug {

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ProcessInfo {
        public int pid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string path;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string titleid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string contentid;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ThreadInfo {
        public int pid;
        public int priority;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string name;
    }

    public class MemoryEntry {
        public ulong end;
        public string name;
        public ulong offset;
        public uint prot;
        public ulong start;
    }

    public class Process {
        public string name;
        public int pid;

        /// <summary>
        /// Initializes Process class
        /// </summary>
        /// <param name="name">Process name</param>
        /// <param name="pid">Process ID</param>
        /// <returns></returns>
        public Process(string name, int pid) {
            this.name = name;
            this.pid = pid;
        }

        public override string ToString() {
            return $"[{pid}] {name}";
        }
    }

    public class ProcessList {
        public Process[] processes;

        /// <summary>
        /// Initializes ProcessList class
        /// </summary>
        /// <param name="number">Number of processes</param>
        /// <param name="names">Process names</param>
        /// <param name="pids">Process IDs</param>
        /// <returns></returns>
        public ProcessList(int number, string[] names, int[] pids) {
            processes = new Process[number];
            for (int i = 0; i < number; i++) {
                processes[i] = new Process(names[i], pids[i]);
            }
        }

        /// <summary>
        /// Finds a process based off name
        /// </summary>
        /// <param name="name">Process name</param>
        /// <param name="contains">Condition to check if process name contains name</param>
        /// <returns></returns>
        public Process FindProcess(string name, bool contains = false) {
            // Iterate trough all the individual processes stored inside the
            // <processes> process list, until either a process whose name
            // contains the or is equal to the value of <name> is found
            foreach (Process p in processes) {
                if (contains) {
                    // If the name of the current process selected from the
                    // process list, contains the value specified by <name>
                    // then return it
                    if (p.name.Contains(name)) 
                        return p;
                } else {
                    // If the current process selected from the process list
                    // name is the same as that specified by <name> then
                    // return it.
                    if (p.name == name)
                        return p;
                }
            }
            // Return null if no process whose name contains the/is equal to
            // the value specified by <name>
            return null;
        }
    }
    public class ProcessMap {
        public MemoryEntry[] entries;
        public int pid;
        /// <summary>
        /// Initializes ProcessMap class with memory entries and process ID
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="entries">Process memory entries</param>
        /// <returns></returns>
        public ProcessMap(int pid, MemoryEntry[] entries) {
            this.pid = pid;
            this.entries = entries;
        }

        /// <summary>
        /// Finds a virtual memory entry based off name
        /// </summary>
        /// <param name="name">Virtual memory entry name</param>
        /// <param name="contains">Condition to check if entry name contains name</param>
        /// <returns></returns>
        public MemoryEntry FindEntry(string name, bool contains = false) {
            // Iterate trough the individual virtual memory entries stored
            // inside of <entries>, until a entry whose name contains or
            // is equal to the value of <name> is found
            foreach (MemoryEntry entry in entries) {
                if (contains) {
                    // If the current entry's name contains the specified
                    // value of <name>, then return that entry
                    if (entry.name.Contains(name)) 
                        return entry;
                } else {
                    // If the current entry's name is equal to the value
                    // specified by <name>, then return that entry
                    if (entry.name == name) 
                        return entry;
                }
            }

            // Return null if no entry whose name either contains or is
            // equal to the value specified by <name>
            return null;
        }

        /// <summary>
        /// Finds a virtual memory entry based off size
        /// </summary>
        /// <param name="size">Virtual memory entry size</param>
        /// <returns></returns>
        public MemoryEntry FindEntry(ulong size) {
            foreach (MemoryEntry entry in entries) {
                if ((entry.start - entry.end) == size) {
                    return entry;
                }
            }

            return null;
        }
    }
}
