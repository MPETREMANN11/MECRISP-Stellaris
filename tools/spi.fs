\ *********************************************************************
\ SPI general library
\    Filename:      spi.fs
\    Date:          01 jan 2024
\    Updated:       01 jan 2024
\    File Version:  1.0
\    Forth:         MECRISP Forth RP2040
\    Board:         Raspberry pico
\    Author:        Marc PETREMANN
\    GNU General Public License
\ *********************************************************************

0 constant spi0
1 constant spi1


spi0 constant PICO_DEFAULT_SPI
\ RX CSN SCK and TX can be modified
16 constant PICO_DEFAULT_SPI_RX_PIN
17 constant PICO_DEFAULT_SPI_CSN_PIN
18 constant PICO_DEFAULT_SPI_SCK_PIN
19 constant PICO_DEFAULT_SPI_TX_PIN




