### O desafio
O objetivo desse desafio é apresentar seus conhecimentos em Python3 sobre APIs RestFULL, operações com bancos de dados, uso de arquivo de configuração, testes automatizados com cobertura, logging, controle de versões, documentação de código e de uso e estrutura/qualidade de código.

A aplicação a ser desenvolvida será o backend para gerenciar listas de tarefas.

Ela deve ser constuída em Python e você pode utilizar um framework (Django/Flask/FastAPI/Pyramid) e o banco de dados de sua preferência.

A API deve conter os seguintes endpoints:

- **/taskList**: retorna todas as lista cadastradas e permite criar uma nova lista. Cada lista possui zero ou mais tarefas.
- **/taskList/id**: permite a edição, alteração e remoção de uma lista específica
- **/tasks**: retorna todas as tarefas de uma lista e permite criar uma nova tarefa. Cada tarefa esta sempre associada à uma lista.
- **/tasks/id**: permite a edição, alteração e remoção de uma tarefa
- **/tags**: retorna todas as tags cadastradas. Cada tag pode estar associada à uma ou mais tarefas.
- **/tags/id**: permite a edição, alteração e remoção de uma tag.

Para assegurar a correta comunicação com um hipotético aplicativo em frontend que gerenciará as tasks, o seguinte contrato de API deve ser seguido para cada model:

- TaskLists
    - Id: uuid
    - Name: string

- Tags
    - Id: uuid
    - Name: string
    - Count: int (O número de tasks utilizando a tag)

- Tasks
    - Id: uuid
    - Title: string
    - Notes: string
    - Priority: integer
    - RemindMeOn: date
    - ActivityType: string (indoors, outdoors)
    - Status: string (open, done)
    - TaskList: uuid
    - Tags: list

### O que esperamos de você:

- Utilize os verbos HTTP (GET, POST, PUT, PATCH, DELETE) corretamente
- Retorne estados HTTP coerentes (200, 404 etc)
- Escreva testes e apresente o relatório de cobertura dos mesmos, afinal precisamos garantir o funcionamento e a qualidade :)
- Escreva documentação do código, suas funções e assinaturas
- Crie logs com classificações (INFO, WARN, ERROR, DEBUG) coerentes
- Utilize virtualização e ferramenta de controle de dependências
- Pense no histórico de remoções (Solução para manter dados históricos deletados; Dados deletados deverão ser preservados)
- Aderência aos padrões de qualidade de código vigentes na comunidade

### Diferenciais:

- Aplicação rodando em ambiente Docker

### Entrega:

- Faça um fork deste repositório para sua conta pessoal no bitbucket, se certifique de que esteja público, e quando finalizar, responda no e-mail do desafio com o link do seu repositório.
