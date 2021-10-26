# Devlog 2 - Montagem da UI

Primeiramente, montei a UI lateral que contém o mascote Nugget, a bolha de texto e o botão de reset. Usando Text Mesh Pro, criei o Font Asset usando a fonte providenciada na pasta de Assets. Inseri os textos nos lugares correspondentes, e criei um Font Asset único para outlines (como no caso do botão de reset).

 Notei que na pasta de sprites existe um botão com dois estados (Padrão e clicado), e então no componente do botão de reset apliquei um simples Sprite Swap quando o jogador clica no botão. Também apliquei bordas no sprite do botão, e no componente `Image` do botão apliquei o `ImageType` como `Sliced`. Fiz isso para que o botão se expandisse em qualquer dimensão sem aparecer esticado de uma forma estranha.

### Esse é o resultado até agora:

![UI1](https://user-images.githubusercontent.com/68963406/138773111-88518a79-dd0c-4077-8626-21317ef0ea21.gif)


E então, montei a UI do inventário. Aplicando o mesmo princípio do `ImageType` para o painel do inventário e os Box Slots, consegui montá-lo sem muito esforço. Criei um Horizontal Layout Group para alinhar os elementos pelo inventário automaticamente. 
Após ter terminado toda a UI, parti para os elementos do mundo - Coloquei um BG da cor especificada e coloquei os encaixes mais ou menos como está na imagem de referência. 

### Esse é o resultado até agora:

![UI2](https://user-images.githubusercontent.com/68963406/138773126-98dc25a4-d14e-42c2-966d-a6657c950ac7.png)

A UI está agora completamente finalizada. Agora, hora de partir para a programação dos elementos da cena. 
