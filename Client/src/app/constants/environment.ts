export const gateway = "https://localhost:7201";

export const catalogEndpoints = {
    items: `${gateway}/catalog/items`,
    types: `${gateway}/catalog/types`,
    brands: `${gateway}/catalog/brands`,
    vendors: `${gateway}/catalog/vendors`
}

export const identityEndpoints = {
    login: `${gateway}/authentication/login`,
    register: `${gateway}/authentication/register`,
    refreshToken: `${gateway}/token/refresh`
}