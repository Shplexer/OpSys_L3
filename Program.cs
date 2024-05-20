using System;
using System.Collections.Generic;

class OpSys_L3 {
    static int CountPageFaults(List<int> pages, int frames, List<int> initialPages) {
        int pageFaults = 0;
        Queue<int> memoryFrames = new(initialPages);
        PrintLine();
        Console.WriteLine($"Алгоритм FIFO для {frames} блоков:");
        PrintLine();
        foreach (int page in pages) {
            // Вывод состояния перед обращением к странице
            Console.WriteLine($"Обращение к странице {page}:");

            if (!memoryFrames.Contains(page)) {
                Console.WriteLine("\tПроисходит страничное прерывание:");
                Console.Write($"\t{FramesToString(memoryFrames)} -> ");
                if (memoryFrames.Count >= frames) {
                    memoryFrames.Dequeue(); // Удалить первую страницу в очереди
                }
                memoryFrames.Enqueue(page); // Добавить новую страницу в конец очереди

                pageFaults++;
                // Вывод состояния после обращения к странице
                Console.WriteLine($"{FramesToString(memoryFrames)}");
            }
            else {
                Console.WriteLine($"\tСтраница уже в памяти: ");
                Console.WriteLine($"\t{FramesToString(memoryFrames)}");

            }

        }

        return pageFaults;
    }
    static int CountPageFaultsLRU(List<int> pages, int frames, List<int> initialPages) {
        int pageFaults = 0;
        List<int> memoryFrames = new List<int>(initialPages);
        LinkedList<int> usageOrder = new LinkedList<int>(initialPages);
        PrintLine();
        Console.WriteLine($"Алгоритм LRU для {frames} блоков:");
        PrintLine();
        foreach (int page in pages) {
            Console.WriteLine($"Обращение к странице {page}:");

            if (!memoryFrames.Contains(page)) {
                Console.WriteLine("\tПроисходит страничное прерывание:");
                Console.Write($"\t{FramesToString(new Queue<int>(memoryFrames))} -> ");
                pageFaults++;
                if (memoryFrames.Count < frames) {
                    memoryFrames.Add(page);
                    usageOrder.AddLast(page);
                }
                else {
                    // Remove the least recently used page
                    int lruPage = usageOrder.First.Value;
                    //Console.WriteLine($"REMOVED {memoryFrames[memoryFrames.IndexOf(lruPage)]} -> {page}" );
                    usageOrder.RemoveFirst();
                    memoryFrames[memoryFrames.IndexOf(lruPage)] = page;
                    usageOrder.AddLast(page);
                }
                Console.WriteLine($"{FramesToString(new Queue<int>(memoryFrames))}");
            }
            else {
                Console.WriteLine($"\tСтраница уже в памяти: ");
                Console.WriteLine($"\t{FramesToString(new Queue<int>(memoryFrames))}");
                // Move the page to the end of usageOrder since it was recently used
                usageOrder.Remove(page);
                usageOrder.AddLast(page);
            }
        }

        return pageFaults;
    }
    static void PrintLine() {
        Console.WriteLine("========================================================");
    }
    static void CompareResults(int pageFaultsFIFO, int pageFaultsLRU, int frames) {
        if (pageFaultsFIFO == pageFaultsLRU) {
            Console.WriteLine($"Алгоритмы FIFO и LRU для {frames} блоков работают одинаково.");
        }
        else if (pageFaultsFIFO < pageFaultsLRU) {
            Console.WriteLine($"Алгоритм FIFO для {frames} блоков более эффективен (количество прерываний: {pageFaultsFIFO}).");
        }
        else {
            Console.WriteLine($"Алгоритм LRU для {frames} блоков более эффективен (количество прерываний: {pageFaultsLRU}).");
        }

    }
    static string FramesToString(Queue<int> frames) {
        return "[" + string.Join(",", frames) + "]";
    }
    static void Main() {
        int frames = 4;
        List<int> pages = [7, 8, 9, 2, 1, 0, 8, 9, 2, 4, 6, 8, 2, 1, 8, 9];
        List<int> initialPages = [8, 2, 9, 6];

        Console.WriteLine($"Начальное состояние блоков: {FramesToString(new Queue<int>(initialPages))}\n");

        int pageFaultsFIFO1 = CountPageFaults(pages, frames, initialPages);
        int pageFaultsFIFO2 = CountPageFaults(pages, frames + 1, initialPages);
        int pageFaultsLRU1 = CountPageFaultsLRU(pages, frames, initialPages);
        int pageFaultsLRU2 = CountPageFaultsLRU(pages, frames + 1, initialPages);
        PrintLine();
        Console.WriteLine($"Количество страничных прерываний алгоритма FIFO для {frames} блоков: {pageFaultsFIFO1}");
        Console.WriteLine($"Количество страничных прерываний алгоритма FIFO для {frames + 1} блоков: {pageFaultsFIFO2}");
        Console.WriteLine($"Количество страничных прерываний алгоритма LRU для {frames} блоков: {pageFaultsLRU1}");
        PrintLine();
        Console.WriteLine($"Количество страничных прерываний алгоритма LRU для {frames + 1} блоков: {pageFaultsLRU2}");
        CompareResults(pageFaultsFIFO1, pageFaultsLRU1, frames);
        CompareResults(pageFaultsFIFO2, pageFaultsLRU2, frames + 1);

    }
}