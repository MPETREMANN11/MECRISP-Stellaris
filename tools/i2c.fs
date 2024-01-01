\ *********************************************************************
\ I2C general library
\    Filename:      i2c.fs
\    Date:          31 dec 2023
\    Updated:       31 dec 2023
\    File Version:  1.0
\    Forth:         MECRISP Forth RP2040
\    Board:         Raspberry pico
\    Author:        Marc PETREMANN
\    GNU General Public License
\ *********************************************************************


0 constant i2c0
1 constant i2c1


i2c1 constant PICO_DEFAULT_I2C
\ SDA and SCL can be modified
4 constant PICO_DEFAULT_I2C_SDA_PIN
5 constant PICO_DEFAULT_I2C_SCL_PIN




\ $4000C000 constant RESETS_BASE    \ 
\ RESETS_BASE $00 + constant RESETS_RESET
\ 
\ : i2c_reset ( i2c -- )
\     1 3 lshift ( RESETS_RESET_I2C0_BITS )  RESETS_RESET bis!
\     1 4 lshift ( RESETS_RESET_I2C1_BITS )  RESETS_RESET bis!
\   ;
\ 
\ 
\ \  Disable the I2C HW block.
\ : i2c_deinit ( i2c -- )
\     i2c_reset
\   ;

: i2c_init ( i2Cx bauds -- )
    \ gpio_set_function(PICO_DEFAULT_I2C_SDA_PIN, GPIO_FUNC_I2C);
    \ gpio_set_function(PICO_DEFAULT_I2C_SCL_PIN, GPIO_FUNC_I2C);
    PICO_DEFAULT_I2C_SDA_PIN GPIO_FUNC_I2C gpio_set_function
    PICO_DEFAULT_I2C_SCL_PIN GPIO_FUNC_I2C gpio_set_function
    \ gpio_pull_up(PICO_DEFAULT_I2C_SDA_PIN);
    \ gpio_pull_up(PICO_DEFAULT_I2C_SCL_PIN);
    PICO_DEFAULT_I2C_SDA_PIN gpio_pull_up
    PICO_DEFAULT_I2C_SCL_PIN gpio_pull_up
  ;

: i2c_write ( data -- )
  ;



