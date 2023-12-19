$d0000000 constant SIO_BASE

SIO_BASE $01c + constant GPIO_OUT_XOR  \ GPIO output value XOR
SIO_BASE $020 + constant GPIO_OE       \ GPIO output enable

25 constant ONBOARD_LED

: blink ( -- )
  1 ONBOARD_LED lshift GPIO_OE !
  begin
    1 ONBOARD_LED lshift GPIO_OUT_XOR !
    300 ms
  key? until
;
