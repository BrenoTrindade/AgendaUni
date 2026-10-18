# AgendaUni

### Organização acadêmica offline com .NET MAUI

Disciplinas, faltas, horários e avaliações em um aplicativo com armazenamento local e lembretes. Desenvolvido em C# para explorar MVVM, persistência com Entity Framework Core e integração com recursos do dispositivo.

**Projeto concluído · Portfólio · Sem manutenção ativa**

O desenvolvimento foi encerrado. Esta versão preserva o aplicativo offline como demonstração técnica, sem contas, servidor ou dependência de uma API.

## O aplicativo

<p align="center">
  <img src="https://github.com/user-attachments/assets/fe6fa44f-4530-4dce-ae08-9d2aa6015f2c" width="250" alt="Captura do AgendaUni: apresentação do aplicativo" />
  <img src="https://github.com/user-attachments/assets/077184c0-71f3-417b-85de-fabaaa09b638" width="250" alt="Captura do AgendaUni: interface mobile" />
  <img src="https://github.com/user-attachments/assets/9ffa66c5-89d1-4a05-8374-1ce56a1ed4f2" width="250" alt="Captura do AgendaUni: organização acadêmica" />
</p>

*Capturas da versão original. O fluxo da demonstração está no [roteiro de apresentação](docs/DEMONSTRACAO.md).*

- **Disciplinas:** cadastro, edição e definição do limite de faltas.
- **Faltas:** registro por disciplina, com data e motivo.
- **Horários:** organização das aulas por dia da semana.
- **Eventos:** provas, trabalhos e outros compromissos acadêmicos.
- **Calendário:** visualização de aulas, eventos e faltas, com filtros.
- **Lembretes locais:** notificações de aulas e eventos, conforme as permissões do dispositivo.
- **Tema claro e escuro:** preferência salva no aparelho.

Os dados ficam no SQLite do dispositivo. Não há sincronização entre aparelhos, recuperação de conta ou backup em nuvem.

## Decisões técnicas

| Escolha | Motivo |
| --- | --- |
| .NET MAUI + XAML | Construção da interface mobile em C# com acesso a recursos nativos. |
| MVVM | Separação entre telas, estado da interface e ações do usuário. |
| EF Core + SQLite | Persistência local e funcionamento offline, sem infraestrutura externa. |
| Serviços e repositórios | Validação dos dados separada das consultas ao banco. |
| Injeção de dependências | Composição das telas e substituição das dependências nos testes. |
| xUnit + Moq | Verificação das regras e do cancelamento de lembretes sem emulador. |

```text
Views (XAML) → ViewModels → Services → Repositories → SQLite
                               ↓
                    INotificationService
                               ↓
                    Notificações do aparelho
```

Detalhes e limites da implementação: [arquitetura](docs/ARQUITETURA.md).

## Executar os testes

Requer apenas o **SDK .NET 9**. O arquivo `global.json` seleciona essa linha do SDK.

```powershell
git clone https://github.com/BrenoTrindade/AgendaUni.git
cd AgendaUni
dotnet test mobile/AgendaUni.Tests/AgendaUni.Tests.csproj --configuration Release
```

Os testes compilam os arquivos reais de modelos, contratos e serviços do aplicativo por referência de código-fonte. Não exigem workloads MAUI, banco instalado, emulador ou servidor. A execução também está configurada no GitHub Actions.

## Executar o aplicativo

O Android é o alvo da demonstração. Use Visual Studio com suporte a .NET MAUI, SDK .NET 9, JDK/Android SDK compatíveis e um emulador ou aparelho com depuração USB.

1. Abra `AgendaUni.sln` no Visual Studio.
2. Selecione `AgendaUni` como projeto de inicialização.
3. Selecione o destino Android e o emulador ou aparelho.
4. Execute em **Debug**.

Pelo terminal, em um ambiente Android já configurado:

```powershell
dotnet build mobile/AgendaUni/AgendaUni.csproj -f net9.0-android -p:TargetFrameworks=net9.0-android -t:Run
```

O banco `absence.db` é criado e migrado automaticamente no primeiro início. Não é necessário instalar SQL Server nem executar comandos de migração manualmente. Ao desinstalar ou limpar os dados do aplicativo, os registros locais são perdidos.

Não há APK ou vídeo de demonstração publicado nesta versão. Os passos para uma apresentação com dados fictícios estão no [roteiro](docs/DEMONSTRACAO.md).

## Escopo encerrado

- A API experimental, o cliente Blazor e o Docker foram retirados desta versão para manter o foco no aplicativo offline. O histórico anterior à revisão de portfólio preserva esses experimentos.
- Não há previsão de lançamento comercial ou novas funcionalidades.
- Notificações dependem das permissões e das políticas de bateria do sistema. Testes unitários não substituem a verificação em um aparelho.
- Os alvos iOS, Mac Catalyst e Windows presentes no projeto são parte da configuração original; não são apresentados como versões homologadas.
- O projeto mantém a stack .NET 9 da implementação original; não há compromisso de atualização de dependências.

## Autor e licença

Desenvolvido por [Breno Trindade](https://github.com/BrenoTrindade).

Código disponibilizado sob a [licença MIT](LICENSE). Dependências e recursos de terceiros mantêm suas próprias licenças.
