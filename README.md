# cipherc

An english-like bytes data process/cipher DSL.

e.g.

```bash
# transform data format
> hex 61626364 print base64

# hash data multiple times
> txt "Welcome to Cipherc" sm3 md5 sha1

# hash multiple times and print intermediate result
> txt "Welcome to Cipherc" sm3 print hex sm3 print hex

# store random data in variable x, then store it's sm3 hash in variable hash_x
> var x is rand 32 then var hash_x is var x sm3
```

See more [wiki/Examples.md](wiki/Examples.md)

## Grammar

See [wiki/Grammar.md](wiki/Grammar.md)

## LICENSE

[MIT](LICENSE)
