# 🎮 Jogo Da Velha (Tic-Tac-Toe)

Aplicação desktop do clássico **Jogo da Velha** desenvolvida em **C# com Windows Forms (.NET 10)**, com suporte a dois modos de jogo: contra a inteligência artificial ou contra outro jogador na mesma máquina.

---

## 📋 Conceito do Jogo

O Jogo da Velha é disputado em um tabuleiro de **3×3 células**. Dois jogadores se alternam colocando seus símbolos — **X** e **O** — nas células vazias. Vence quem completar primeiro uma linha de três símbolos iguais na horizontal, vertical ou diagonal. Se todas as nove células forem preenchidas sem um vencedor, a partida termina em **empate**.

---

## 🕹️ Modos de Jogo

Ao iniciar a aplicação, uma caixa de diálogo solicita ao jogador que escolha o modo desejado:

| Opção | Modo | Descrição |
|-------|------|-----------|
| **Sim** | SinglePlayer | O jogador (X) enfrenta a Inteligência Artificial (O) |
| **Não** | MultiPlayer | Dois jogadores se alternam na mesma máquina (X e O) |

### SinglePlayer
- O jogador humano utiliza sempre o símbolo **X**.
- Após cada jogada do jogador, a IA assume automaticamente o turno com um breve atraso visual.
- O placar exibe **Player** (verde) vs **AI** (vermelho).

### MultiPlayer
- Os dois jogadores se alternam manualmente clicando nas células.
- O título da janela indica de quem é a vez: `"Tic Tac Toe - Vez do Jogador X/O"`.
- O placar exibe **Jogador X** (azul) vs **Jogador O** (vermelho).

---

## ✨ Funcionalidades

- **Seleção de modo** ao iniciar: diálogo de escolha entre SinglePlayer e MultiPlayer.
- **Tabuleiro visual** com 9 botões (3×3) inicialmente exibindo `?`, substituídos por `X` ou `O` a cada jogada.
- **Destaque por cores**:
  - SinglePlayer: jogadas do Player em **ciano** e da IA em **salmão escuro**.
  - MultiPlayer: jogadas do Jogador X em **azul claro** e do Jogador O em **coral claro**.
- **Detecção de vitória** em todas as 8 combinações possíveis (3 linhas, 3 colunas e 2 diagonais).
- **Detecção de empate** quando todas as células são preenchidas sem vencedor.
- **Placar por sessão** que acumula vitórias de cada jogador enquanto a aplicação está aberta.
- **Reset automático** do tabuleiro após cada partida (com 1 segundo de delay após uma vitória).
- **Botão de reinício** que limpa o tabuleiro e mantém o placar acumulado.

---

## 🤖 Inteligência Artificial

A IA (`TicTacToeAI`) utiliza uma estratégia baseada em prioridades, avaliando o tabuleiro a cada turno:

1. **Vencer** — Se a IA puder completar três em linha na próxima jogada, faz isso imediatamente.
2. **Bloquear** — Se o jogador humano estiver a uma jogada de vencer, a IA bloqueia.
3. **Centro** — Se a célula central (posição 1,1) estiver livre, a IA a ocupa.
4. **Cantos** — A IA escolhe aleatoriamente um dos cantos disponíveis.
5. **Qualquer célula livre** — Como último recurso, escolhe aleatoriamente entre as posições restantes.

---

## 🗂️ Estrutura da Aplicação

```
tictactoeXO/tictactoe-master/
│
├── Program.cs                  # Ponto de entrada da aplicação
├── PlayerInfo.cs               # Modelo de dados do jogador (nome, símbolo, vitórias, isAI)
│
├── Game/
│   ├── GameBoard.cs            # Lógica do tabuleiro 3×3 (get/set células, verificar vitória, células livres, clone)
│   ├── GameController.cs       # Controle do fluxo de jogo (turnos, IA, vitórias, empate, placar)
│   └── GameMode.cs             # Enum com os modos: SinglePlayer / MultiPlayer
│
├── IA/
│   └── TicTacToeAI.cs          # Algoritmo de IA (vencer > bloquear > centro > cantos > aleatório)
│
└── Interface/
    ├── Form1.cs                # Formulário principal: eventos de clique, lógica de UI e integração com Game
    └── Form1.Designer.cs       # Código gerado pelo WinForms Designer (layout dos controles)
```

### Descrição das classes principais

| Classe | Responsabilidade |
|--------|-----------------|
| `GameBoard` | Mantém o estado do tabuleiro, valida posições, detecta vitória e empate, gera clones para simulação |
| `GameController` | Orquestra as partidas: gerencia turnos, aciona a IA, registra vitórias e controla o estado da sessão |
| `TicTacToeAI` | Implementa a lógica de decisão da IA com prioridades estratégicas |
| `PlayerInfo` | Armazena nome, símbolo (X ou O), contagem de vitórias e flag de IA |
| `Form1` | Camada de interface gráfica com os 9 botões do tabuleiro, labels de placar e timer da IA |

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# 13
- **Framework:** .NET 10 (Windows)
- **UI:** Windows Forms (WinForms)
- **IDE recomendada:** Visual Studio 2022+

---

## 📐 Diagramas

### Diagrama de Casos De Uso — SinglePlayer

<img width="1037" height="489" alt="image" src="https://github.com/user-attachments/assets/23746fc2-0f2b-4ca0-b578-9d3770afcd06" />

### Diagrama de Casos De Uso — MultiPlayer

<img width="628" height="453" alt="image" src="https://github.com/user-attachments/assets/6cfa1c46-e8bd-4f1c-a269-b892ce4e3655" />

### Diagrama de Classe

<img width="1004" height="692" alt="image" src="https://github.com/user-attachments/assets/da2a464c-4b18-4757-808c-d1e8048525da" />

### Gestão de Projeto — Trello

<img width="1431" height="792" alt="image" src="https://github.com/user-attachments/assets/a408585a-e98b-41fe-b689-a5d259cba4e7" />

