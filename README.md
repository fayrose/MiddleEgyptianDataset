# Middle Egyptian Dictionary Parser
Parses and combines 3 Middle Egyptian dictionaries (Mark Vygus (2012, 2018), Paul Dickson, and a third lexicon originally from OpenGlyph (that I found through the Morris Franken dataset for "Automatic Egyptian Hieroglyph Recognition by Retrieving Images as Texts")) for addition to a database.


# Stages of the Project Thus Far:
1. Read in PDF files from Vygus, Dickson, Lexicon
2. Parse different display formatting and clean up
3. Display as unicode
4. Realize unicode is all unformatted -- implement a trigram model to add formatting lost when PDFs read in

	4.1 - Given formatted texts, preprocess and parse all formatted trigrams
    
	4.2 - Map formatted trigrams back to words in dictionary
    
5. Add caching and serialization to trigram model to speed up database generation
6. Attempt to implement formatted unicode only to realize there are no fonts for this aside from the Unicode 12 spec. Migrate application to RESJs, which takes somewhat longer to render but allows for glyph formatting.
7. Added formatted transliteration where dotted h's display in lieu of a capital H and such.
8. Work on improving parts of speech, which weren't standardized between the two texts
9. Realize that keyword search for translation is slow, but Mongo text indexing is not working. Create own keyword indexer for application.

	9.1 - Iterate over all dictionary entries' translations, remove stop words and file entry under key words
    
	9.2 - When a search is conducted, remove stop words from translation, and then conduct pre-performed searches of remaining words. Intersect or union returned entries based on user configurations.
    
10. Add an advanced search field over the gardiner signs that displays signs as the user searches to help new users onboard.
11. Added a Gardiner Sign List description page.

# Planned Stages of the Project Going Forward:
1. Add mobile responsitivity
2. Add log-in with 2 types of user - admin & editor

	2.1 - Editors can make approved appropriate changes to formatting pending admin approval
    
	2.2 - Admins can view a queue of requested changes and approve or deny
    
	2.3 - Changes are not pushed to the database until the entire queue is viewed, to prevent unnecessary expense
    
3. Begin working on tagger
4. Begin working on translation scheme.

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
