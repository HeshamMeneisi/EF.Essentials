class SemanticReleaseError extends Error {
    constructor(message, code, details) {
        super(message);
        Error.captureStackTrace(this, this.constructor);
        this.name = 'SemanticReleaseError';
        this.details = details;
        this.code = code;
        this.semanticRelease = true;
    }
}

const PACKAGE_NAME = process.env.PACKAGE_NAME;
const GH_TOKEN = process.env.GH_TOKEN;
const MANAGE_PACKAGES_TOKEN = process.env.MANAGE_PACKAGES_TOKEN;
const GITHUB_USER = process.env.GITHUB_USER;

module.exports = {
    verifyConditions: [
        () => {
          for(let arg of ['GH_TOKEN', 'PACKAGE_NAME', 'MANAGE_PACKAGES_TOKEN', 'GITHUB_USER']){
            if(!process.env[arg]){
              throw new SemanticReleaseError(
                `No ${arg} specified`,
                "E_MISSING_ENV_VARIABLE",
                `Please make sure to set ${args} on your CI environment.`);
            }
          }
        },
        "@semantic-release/github"
    ],
    prepare: [
      {
        path: "@semantic-release/exec",
        cmd: `dotnet pack ${PACKAGE_NAME}/${PACKAGE_NAME}.csproj --configuration Release /p:Version=\${nextRelease.version}`
      }
    ],
    publish: [
      {
        path: "@semantic-release/exec",
        cmd: `dotnet nuget push "${PACKAGE_NAME}/bin/Release/*.nupkg" -k ${MANAGE_PACKAGES_TOKEN} -s https://nuget.pkg.github.com/${GITHUB_USER}/index.json `
      },
      "@semantic-release/github"
    ]
};
