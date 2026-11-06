# Laboratory Work No. 2 - Cryptanalysis of monoalphabetic ciphers

Variant **19**, Mihail Pancenco, FAF-243.

- `report/main.pdf`: compiled report.
- `report/main.tex`: editable LaTeX report using the Lab 1 template.
- `report/data/ciphertext.txt`: complete working ciphertext.
- `report/data/plaintext_readable.txt`: complete recovered message with restored word spacing.
- `report/data/plaintext_exact.txt`: exact substitution of the working ciphertext.
- `report/data/ciphertext_supplied.txt` and `plaintext_supplied_exact.txt`: original pasted ciphertext and its exact decryption, retaining extraction artifacts.
- `report/data/substitution_key.txt`: decryption alphabet; M -> Z is inferred by elimination because M never occurs.
- `report/data/verification.json`: verification results.

The report records the first seven substitutions from the supplied notes and explains a continuation that recovers the remaining letters. The online frequency-analysis service confirms the final result. All 3,230 plaintext letters re-encrypt correctly.

No lab code is required or included. Compile the report from `report/` with `pdflatex main.tex` twice to resolve references.
