﻿using System.Text.Json.Serialization;
using BL;

namespace Logic;

internal class Folder : IFolder
{
    [JsonConstructor]
    public Folder(string name, List<Note> notes)
    {
        Name = name;
        Notes = notes;
    }
    
    public Folder(IFolder folder)
    {
        Name = folder.Name;
        Notes = folder.Notes.Select(n => new Note(n)).ToList();
    }

    internal Folder(string name)
    {
        Name = name;
        Notes = new List<Note>();
    }

    public string Name { get; }
    
    IEnumerable<INote> IFolder.Notes => Notes.AsReadOnly();

    public List<Note> Notes { get; }
}