# Laboratory Work No. 3 - Romanian Playfair Cipher

Mihail Pancenco, FAF-243. Number **19** selects task **3.1 (Playfair)**.

- [Compiled PDF report](report/main.pdf)
- [Editable LaTeX report](report/main.tex)
- [C# console project and instructions](code/playfair/README.md)
- [Captured console results and verification](report/evidence/verification.json)

The implementation uses a six-row, five-column matrix, merges J into I, and preserves Î separately. It contains only Program and PlayfairCipher. The report explains normalization, matrix construction, pair preparation, all three encryption/decryption rules, validation, and manually checked examples.

To run the program, open a terminal in `code/playfair/` and run `dotnet run`.

To compile the report, open a terminal in `report/` and run `pdflatex main.tex` twice. The generated PDF is **lab_03/report/main.pdf** relative to the repository root.
