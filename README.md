# cipherc [WIP]

A cross platform app to transform data fromat or do cryptographic operation.

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

## Import data `from`

```bash
cipherc from <FORMAT> <SOURCE> [<ARG>]
```

`<FORMAT>`: txt, hex, base64, bin, pem

`<SOURCE>`: arg|data, file|path, pipe|stdin

## Generate random data `rand`

```bash
cipherc rand <N> bit
```

## Transform format `to`

```bash
cipherc <DATA> to <FORMAT> <SOURCE> [<ARG>]
```

## Pipe support

```bash
echo from base64 txt YWJjZA== | cipherc
```

## `Sentence`

cipherc sm2 gen sign from hex 010203

cipherc sm2 load key from hex 010203 get pk to hex

cipherc sm4-gcm 
