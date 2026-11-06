# Romanian Playfair Cipher

Laboratory Work No. 3, task 3.1, variant 19. Requires .NET 10.

Run from this directory:

```sh
dotnet run
```

Choose encryption or decryption, enter a keyword of at least seven Romanian letters, then enter the message or ciphertext. Keywords contain letters only; messages may also contain ordinary spaces. The program identifies invalid characters and asks again.

The matrix has six rows and five columns. J is normalized to I; Î remains a separate letter. The unique keyword letters are written first, followed by the remaining Romanian letters in alphabetical order. Uppercase, lowercase and Romanian cedilla variants are supported.

Output shows the normalized key, unique key letters, matrix, normalized message, filler insertions, prepared pairs, and one short line per pair: input, output, and the rule used.

Encryption inserts X between identical letters inside a pair and after an unmatched final letter. If the letter needing a filler is X, Q is used. Decryption reads pairs directly and retains fillers. Resolve fillers and restore spaces manually; the original I/J choice and capitalization cannot be recovered automatically.

## Example

Use keyword **AĂÂBCDE** to obtain the matrix in the agreed alphabet order.

- Message: **BALLOON**
- Prepared pairs: **BA LX LO ON**
- Ciphertext: **CĂKYÎQPO**
- Decryption with the same keyword: **BALXLOON**

## Classes

- `Program`: menu, input, validation retries and the final result.
- `PlayfairCipher`: normalization, key validation, matrix construction, pair preparation, encryption and decryption. It prints a short explanation directly where each operation happens.
