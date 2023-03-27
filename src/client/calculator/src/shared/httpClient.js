const request = async (method, url, data) => {
    try {
        return await fetch(url, createContent(method, data));
    } catch (error) {
        throw new Error(error.message);
    }
};

const createContent = (method, data) => {
    const options = {
        method,
        headers: {}
    };

    if (data) {
        options.headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(data);
    }

    return options;
};

export const get = request.bind(null, 'GET');
export const post = request.bind(null, 'POST');
export const put = request.bind(null, 'PUT');
export const del = request.bind(null, 'DELETE');
