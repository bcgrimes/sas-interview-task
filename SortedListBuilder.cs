using System;
using System.Collections.Generic;
using sas_interview_task.models;

namespace sas_interview_task.tools;

public class SortedListBuilder
{
    // Constructors

    public SortedListBuilder(string primarySortColumn, bool primarySortASC = true, string secondarySortColumn = "")
    {
        PrimarySortColumn = primarySortColumn;
        PrimarySortASC = primarySortASC;
        SecondarySortColumn = secondarySortColumn;
    }

    // Properties

    public string PrimarySortColumn { get; set; } = "";
    public bool PrimarySortASC { get; set; } = true;
    public string SecondarySortColumn { get; set; } = "";

    // Methods

    // Sort method with options parameters
    public void Sort(Invoice[] collection, string primarySortColumn, bool primarySortASC = true, string secondarySortColumn = "")
    {
        PrimarySortColumn = primarySortColumn;
        PrimarySortASC = primarySortASC;
        SecondarySortColumn = secondarySortColumn;
        Sort(collection);
    }

    // Sort method, relying on existing sort option property values
    public void Sort(Invoice[] collection)
    {
        // Only sort if at least the primary sort column is defined
        if (PrimarySortColumn.Length > 0)
        {
            collection = QuickSort(collection, 0, collection.Length - 1);
            if (!PrimarySortASC)
            {
                Array.Reverse(collection);
            }
        }
    }

    // Invoice column comparison internal method
    private int CompareColumns(Invoice left, Invoice right)
    {
        int primaryCompare = left.CompareTo(right, PrimarySortColumn);
        if (SecondarySortColumn.Length > 0 && primaryCompare == 0)
        {
            return left.CompareTo(right, SecondarySortColumn);
        }
        return primaryCompare;
    }

    // Quicksort internal method (https://en.wikipedia.org/wiki/Quicksort)
    private Invoice[] QuickSort(Invoice[] collection, int leftIndex, int rightIndex)
    {
        int left = leftIndex;
        int right = rightIndex;
        Invoice pivotItem = collection[leftIndex]; // Initially the first array item

        // Check that the tracking indexes do not cross
        while (left <= right)
        {
            // Traverse from left over items less than the pivot
            while (CompareColumns(collection[left], pivotItem) < 0) // ASC
            {
                left++;
            }
            // Traverse from right over items greater than the pivot
            while (CompareColumns(collection[right], pivotItem) > 0) // ASC
            {
                right--;
            }
            // After traversing, swap left and right items if needed
            if (left <= right)
            {
                Invoice temp = collection[left];
                collection[left] = collection[right];
                collection[right] = temp;
                left++;
                right--;
            }
        }
        // Check left side if further sorting needed
        if (leftIndex < right)
        {
            QuickSort(collection, leftIndex, right);
        }
        // Check right side if further sorting needed
        if (left < rightIndex)
        {
            QuickSort(collection, left, rightIndex);
        }

        return collection;
    }
}
