/**
 * Convert a `File` object returned by the upload input into a base 64 string.
 */
export const convertFileToBase64 = (file) =>
    new Promise((resolve, reject) => {
        const reader = new FileReader()
        // Assign resolve
        reader.onload = () => resolve(reader.result)
        // Assign reject
        reader.onerror = reject

        // Try read file
        reader.readAsDataURL(file.rawFile)
    })

const transformImagesAndRespond = (action, restDataProvider, resource, params) => {
    // Freshly dropped images are File objects and must be converted to base64 strings
    const newImages = params.data.images.filter((p) => p.rawFile instanceof File)
    const formerImages = params.data.images.filter((p) => !(p.rawFile instanceof File))

    return Promise.all(newImages.map(convertFileToBase64))
        .then((base64Pictures) => transformImages(base64Pictures, params))
        .then((transformedNewImages) => {
            return action === 'create'
                ? restDataProvider.create(
                    resource,
                    transformedImagesReducer(transformedNewImages, formerImages, params)
                )
                : restDataProvider.update(
                    resource,
                    transformedImagesReducer(transformedNewImages, formerImages, params)
                )
        })
}

const transformImages = (base64Pictures, params) => {
    // Transform converted images to new object
    return base64Pictures.map((picture64, i) => ({
        base64String: picture64,
        title: `${params.data.images[i].title}`,
    }))
}

const transformedImagesReducer = (transformedNewImages, formerImages, params) => ({
    ...params,
    data: {
        ...params.data,
        // Add images to the initial params
        images: [...transformedNewImages, ...formerImages],
    },
})

export default transformImagesAndRespond
