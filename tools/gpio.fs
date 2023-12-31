\ *********************************************************************
\ GPIO general library
\    Filename:      gpio.fs
\    Date:          27 dec 2023
\    Updated:       01 jan 2024
\    File Version:  1.4
\    Forth:         MECRISP Forth RP2040
\    Board:         Raspberry pico
\    Author:        Marc PETREMANN
\    GNU General Public License
\ *********************************************************************

\ Essentials words:

\ gpio_set_function ( gpio function -- )
\  Parameters:
\  - gpio: between [2..30]
\  - function: 
\     GPIO_FUNC_SPI
\     GPIO_FUNC_UART
\     GPIO_FUNC_I2C 
\     GPIO_FUNC_PWM
\     GPIO_FUNC_SIO
\     GPIO_FUNC_PIO0
\     GPIO_FUNC_PIO1
\     GPIO_FUNC_GPCK
\     GPIO_FUNC_USB
\     GPIO_FUNC_NULL

\ gpio_set_dir    ( gpio direction -- ) 
\  Parameters:
\  - gpio: between [2..30]
\  - direction: GPIO-OUT | GPIO-IN 

\ gpio_put ( gpio state -- )
\  Parameters:
\  - gpio: between [2..30]
\  - state: GPIO-HIGH | GPIO-LOW


compiletoflash

30 constant NUM_BANK0_GPIOS

\ test GPIO and abort if gpio num > 30
\ abort program if GPIO num is bad
: check_gpio_param ( gpio -- )
    NUM_BANK0_GPIOS > if
        ." GPIO num. error!" quit
    then
  ;


$D0000000 constant SIO_BASE
SIO_BASE $020 + constant GPIO_OE
SIO_BASE $024 + constant GPIO_OE_SET
SIO_BASE $028 + constant GPIO_OE_CLR

SIO_BASE $004 + constant GPIO_IN        \ Input value for GPIO pins [0..30]

SIO_BASE $010 + constant GPIO_OUT       \ GPIO output value
SIO_BASE $014 + constant GPIO_OUT_SET   \ GPIO output value set
SIO_BASE $018 + constant GPIO_OUT_CLR   \ GPIO output value clear
SIO_BASE $01c + constant GPIO_OUT_XOR   \ GPIO output value XOR


\ transform GPIO number in his binary mask
: PIN_MASK ( n -- mask )
    1 swap lshift
  ;

1 constant GPIO-OUT   \ set direction OUTput mode
0 constant GPIO-IN    \ set direction INput mode

\ set direction for selected gpio
: gpio_set_dir  ( gpio direction -- )
    if     PIN_MASK GPIO_OE_SET !
    else   PIN_MASK GPIO_OE_CLR !    then
  ;

1 constant GPIO_HIGH    \ set GPIO state
0 constant GPIO_LOW     \ set GPIO state

\ set GPIO on/off
: gpio_put ( gpio state -- )
    if     PIN_MASK GPIO_OUT_SET !
    else   PIN_MASK GPIO_OUT_CLR !    then
  ;

\ Get state of a single specified GPIO 
: gpio_get ( gpio -- state )
    PIN_MASK 
    GPIO_IN @  and
  ;

$40014000 constant IO_BANK0_BASE

: GPIO_CTRL ( n -- addr ) 
    8 * 4 + IO_BANK0_BASE + 
  ;

 1 constant GPIO_FUNC_SPI
 2 constant GPIO_FUNC_UART
 3 constant GPIO_FUNC_I2C 
 4 constant GPIO_FUNC_PWM
 5 constant GPIO_FUNC_SIO
 6 constant GPIO_FUNC_PIO0
 7 constant GPIO_FUNC_PIO1
 8 constant GPIO_FUNC_GPCK
 9 constant GPIO_FUNC_USB
$f constant GPIO_FUNC_NULL

: gpio_set_function ( gpio function -- )
    over check_gpio_param       \ abort if bad GPIO num
    swap GPIO_CTRL !
  ;

$1F constant IO_BANK0_GPIO0_CTRL_FUNCSEL_BITS

: gpio_get_function ( gpio -- function )
    GPIO_CTRL @
    IO_BANK0_GPIO0_CTRL_FUNCSEL_BITS and
  ;


\ Initializes the GPIOx peripheral in GPIO_IN direction
: gpio_init ( gpio -- )
    >r
    r@ GPIO_IN gpio_set_dir
    r@ GPIO_LOW gpio_put
    r> GPIO_FUNC_SIO gpio_set_function
  ;


\ Deinitializes the GPIOx peripheral registers to their default reset values
: gpio_deinit ( gpio -- )
    GPIO_FUNC_NULL gpio_set_function
  ;


$4001c000 constant PADS_BANK0_BASE  \  User Bank Pad Control registers 

: PAD_CTRL ( n -- addr ) 
    4 * 4 + PADS_BANK0_BASE + 
  ;


$00000040 constant PADS_BANK0_GPIO0_IE_BITS

\ used to enable/disable the input at the pad
: gpio_set_input_enabled ( gpio enabled -- )
    if
        PADS_BANK0_GPIO0_IE_BITS swap PAD_CTRL bis!
    else
        PADS_BANK0_GPIO0_IE_BITS swap PAD_CTRL bic!
    then
  ;


3 constant PADS_BANK0_GPIO0_PUE_LSB
2 constant PADS_BANK0_GPIO0_PDE_LSB

: gpio_set_pulls ( gpio up down -- )    \ @TODO: à vérifier
    rot >r
    r@ check_gpio_param         \ abort if num GPIO is bad
    \ Pull down enable
    if    PADS_BANK0_GPIO0_PDE_LSB r@ PAD_CTRL bis!
    else  PADS_BANK0_GPIO0_PDE_LSB r@ PAD_CTRL bic!    then
    \ Pull up enable
    if    PADS_BANK0_GPIO0_PUE_LSB r@ PAD_CTRL bis!
    else  PADS_BANK0_GPIO0_PUE_LSB r@ PAD_CTRL bic!    then
    rdrop
  ;

\ Disable pulls on specified GPIO
: gpio_disable_pulls ( gpio -- )    \ @TODO: à vérifier
    0 0 gpio_set_pulls
  ;

\ Set specified GPIO to be pulled down
: gpio_pull_down ( gpio -- )    \ @TODO: à vérifier
    0 1 gpio_set_pulls
  ;

\ Set specified GPIO to be pulled up
: gpio_pull_up ( gpio -- )    \ @TODO: à vérifier
    1 0 gpio_set_pulls
  ;


$4000C000 constant RESETS_BASE    \ reset control
RESETS_BASE $00 + constant RESETS_RESET
RESETS_BASE $04 + constant RESETS_WDSEL
RESETS_BASE $08 + constant RESETS_RESET_DONE


save
compiletoram

