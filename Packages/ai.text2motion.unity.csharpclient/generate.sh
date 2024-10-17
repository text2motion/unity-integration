# To obtain the latest text2motion api specification, go to https://developer.text2motion.ai/
# Log in, click on APIs section on the upper right, and DOWNLOAD SPEC button on the upper right.
# Run ./generate.sh to generate the csharpclient

# Delete old files
rm -rf ./Text2MotionClientAPI
rm -rf ./Text2MotionClientAPI.Test
rm -rf ./docs
rm ./README.md

# Generate the file using openapi-generator-cli from docker
docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli generate \
    -i /local/text2motionapifreetier.yaml \
    -g csharp \
    -o /local/.tmp \
    --additional-properties targetFramework=netstandard2.1,apiName=Text2MotionClientAPI,packageName=Text2MotionClientAPI,library=unityWebRequest

# Move the generated files to the correct location
sudo chown -R $USER:$USER ./.tmp
mv ./.tmp/src/Text2MotionClientAPI ./Text2MotionClientAPI
mv ./.tmp/src/Text2MotionClientAPI.Test ./Text2MotionClientAPI.Test
mv ./.tmp/README.md ./README.md
mv ./.tmp/docs ./docs

# Delete the temporary folder
rm -rf ./.tmp

# Patches
sed -i -e 's/AbstractOpenAPISchema,/AbstractOpenAPISchema/g' ./Text2MotionClientAPI/Model/LocationInner.cs