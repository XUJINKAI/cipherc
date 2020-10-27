# cipherc

A cross platform app to transform data fromat or do cryptographic operation.

**WIP: not all designs are available now, see Unit Tests for demos.**

e.g.

```bash
# transform data format
cipherc from hex data 61626364 to base64 data

# hash data
cipherc sm3 from txt data "to be hash"

# output random 32bit data to hex format and bin file
cipherc rand 32 bit \
    to hex data \
    to bin file "rand.bin"

# generate sm2 key pair then sign data
cipherc sm2 gen \
    sign from txt data "secret content" \
        to hex data

# sm4 enc and dec data
cipherc sm4 \
    set key from bin file "1.key" \
    set iv from hex data "000000" \
    enc ecb from txt data "abcdef" \
        to var enc_result \
    dec ecb from var enc_result \
        to base64 data

# variable and multiple expression
cipherc \
    sm2 set key from hex data "123456" \
        get pk to var x \
    then \
    sm3 from var x

# recursive expression
cipherc sm3 \
    sm2 \
        set key from hex data "123456" \
        get pk to out
```

## Grammar

```c
Block -> Sentence { then Sentence }
Sentence -> Expression | Statement | Declaration

// Expression

Expression -> DataExpression

DataExpression -> DataTerm { concat DataTerm }      // plus

DataTerm -> DataFactor { times <N> }                // multiply

DataFactor -> PostfixData
// 先计算前缀，再计算后缀
PostfixData -> PrefixData { DataOperator | print PrintFormat }

PrefixData ->  { DataOperator } DataPrimary

DataPrimary ->
          InputSource <input>               // file <Path> | var <VAR> | rand <N-bytes>
        | pipe                              // txt <pipeInputString>

DataOperator ->
        | encode EncodeFormat
        | decode DecodeFormat
        | HashOperator
        | sub <Nstart> <Nlength>

InputSource  -> txt | hex | base64 | file | var | rand
PrintFormat  -> txt | hex | base64
EncodeFormat -> txt | hex | base64 | url
DecodeFormat -> txt | hex | base64 | url | pem
HashOperator -> sm3 | md5 | sha1 | sha256

// Statement

Statement ->
          ObjectSentence
        | print PrintFormat DataExpression

ObjectSentence ->
        | Object get <KEY>
        | Object set <KEY> Data
        | Object ObjectAction <PARAM> Data

Asym -> sm2 | rsa1024
Sym  -> sm4 | aes
Pack -> x509 | p10
Object -> Asym | Sym | Pack
ObjectAction -> enc | dec | sign | check

// Declaration

Declaration -> Assignment | FuncDeclaration

Assignment ->
          var <ID> is DataExpression
        | obj <ID> is Object

FuncDeclaration -> ε

```
