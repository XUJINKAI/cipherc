# cipherc

An english-like bytes data process/cipher DSL.

e.g.

```bash
# transform data format
> hex 61626364 print base64

# hash data multiple times
> txt "WelcomeCipherc" sm3 md5 sha1

# hash multiple times and print intermediate result
> txt "WelcomeCipherc" sm3 print hex sm3 print hex

# store random data in variable x, then store it's sm3 hash in variable hash_x
> var x is rand 32 then var hash_x is var x sm3

# use command vars to list all variables
> vars
```

## Grammar

```c
Block -> Sentence { then Sentence }
Sentence ->   DataExpression
            | var <VAR> is DataExpression
            | vars
            | help

// Expression

DataExpression -> DataFactor { concat DataFactor }      // +

// 对DataPrimary附加DataOperator运算
// 先向前计算，再向后计算
DataFactor ->  PostfixData
PostfixData -> PrefixData { DataOperator }
PrefixData ->  DataOperator PrefixData | DataPrimary

DataOperator ->
        | encode EncodeFormat
        | decode DecodeFormat
        | HashOperator
        | repeat <times>            // *
        | sub <Nstart> <Nlength>
        | print PrintFormat         // contains no space
        | printf PrintFormat        // print readable
        | to file <Path>
        | to var <VAR>

DataPrimary -> hex <HEX> | bin <BIN> | base64 <BASE64> | url <URL>
            | txt <UTF8> | var <VAR> | rand <N-bytes> | file <PATH>
            | pipe txt          // pipe input is text
            | pipe file         // pipe input is file path

PrintFormat  -> hex | bin | base64 | url | txt | ascii | auto
EncodeFormat -> hex | bin | base64 | url
DecodeFormat -> hex | bin | base64 | url | pem
HashOperator -> sm3 | md5 | sha1 | sha256 | sha384 | sha512 | sha3

// WIP...

ObjectSentence ->
        | Object get <KEY>
        | Object set <KEY> Data
        | Object ObjectAction <PARAM> Data

Asym -> sm2 | rsa1024
Sym  -> sm4 | aes
Pack -> x509 | p10
Object -> Asym | Sym | Pack
ObjectAction -> enc | dec | sign | check
```
