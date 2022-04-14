const baseUrl = '/api/video';

export const getAllVideos = () => {
    return fetch(`${baseUrl}/GetWithComments`)
        .then(res => res.json())
};

export const searchVideos = (searchTerm, sortDesc) => {
    return fetch(`${baseUrl}/search?q=${searchTerm}&sortDesc=${sortDesc}`)
        .then(res => res.json())
};

export const addVideo = (video) => {
    return fetch(baseUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(video),
    });
};

export const getVideo = (id) => {
    return fetch(`${baseUrl}/GetWithComments/${id}`).then(res => res.json());
};

export const getUserWithVideos = (id) => {
    return fetch(`/api/UserProfile/GetProfileWithVideos/${id}`).then(res => res.json());
};