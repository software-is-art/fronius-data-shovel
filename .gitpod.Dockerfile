FROM gitpod/workspace-full

USER gitpod

RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    sudo dpkg -i packages-microsoft-prod.deb

RUN sudo apt-get update && \
    sudo apt-get install -y apt-transport-https && \
    sudo apt-get update && \
    sudo apt-get install -y dotnet-sdk-5.0 && \
    sudo apt-get install -y dotnet-sdk-3.1

ENV PATH "$PATH:$HOME/.dotnet/tools"
RUN dotnet tool install -g Amazon.Lambda.Tools
RUN dotnet tool install -g dotnet-format
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
    unzip awscliv2.zip && \
    sudo ./aws/install && \
    rm awscliv2.zip
RUN brew install node@12
RUN npm install -g aws-cdk

RUN cd $HOME && \
    wget https://github.com/balena-io/balena-cli/releases/download/v12.33.1/balena-cli-v12.33.1-linux-x64-standalone.zip && \
    unzip balena-cli-v12.33.1-linux-x64-standalone.zip && \
    rm balena-cli-v12.33.1-linux-x64-standalone.zip

ENV PATH "$PATH:$HOME/balena-cli"

