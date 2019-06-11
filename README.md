# Middle Egyptian Dictionary Parser
Parses and combines 3 Middle Egyptian dictionaries (Mark Vygus (2012, 2018), Paul Dickson, and a third lexicon originally from OpenGlyph (that I found through the Morris Franken dataset for "Automatic Egyptian Hieroglyph Recognition by Retrieving Images as Texts")) for addition to a database.

This repository also contains the Trigram model where I aim to combine the list of Gardiner codes with its formatted Manuel de Codage in order to allow for formatted glyphs on the website. While the spec for Unicode 12.0 has been released, it appears that it has not been formally implemented. As a workaround, an additional field has been added to convert the Manuel de Codage to RES (https://mjn.host.cs.st-andrews.ac.uk/egyptian/res/js/), which can then be rendered via Javascript. 


## Initial Documentation

To Create a Singular Dictionary:

    Dictionary<string, DictionaryEntry> entries = new Dictionary<string, DictionaryEntry>();
    VygusFactory fact = new VygusFactory();
    fact.Create2018Instance(entries).ParseAll();

To Create All Dictionaries:

    MiddleEgyptianDictionary med = new MiddleEgyptianDictionary();
    med.CreateDictionaries();
    Console.WriteLine("Dictionaries created!");

Write Dictionaries to Database:

    var task = Task.Run(async () => { await DbWriter.WriteToDbAsync(med); });
    task.Wait();
    Console.WriteLine("Dictionaries added to database!");

< More to add on Trigram model and caching >
